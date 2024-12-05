
#include "accel.h"
#include "util.h"
#include "float.h"
#include "config.h"
#include <linux/kernel.h>
#include <linux/module.h>
#include <linux/time.h>
#include <linux/string.h>
#include <linux/version.h>
#if LINUX_VERSION_CODE < KERNEL_VERSION(5,0,0)
    #include <asm/i387.h>
#else
    #include <asm/fpu/api.h>
#endif

MODULE_AUTHOR("azolotukhin");

#define _s(x) #x
#define s(x) _s(x)

#define PARAM_F(param, default, desc)                           \
    float g_##param = default;                                  \
    static char* g_param_##param = s(default);                  \
    module_param_named(param, g_param_##param, charp, 0644);    \
    MODULE_PARM_DESC(param, desc);

#define PARAM(param, default, desc)                             \
    static char g_##param = default;                            \
    module_param_named(param, g_##param, byte, 0644);           \
    MODULE_PARM_DESC(param, desc);


PARAM(no_bind,          0,              "This will disable binding to this driver via 'leetmouse_bind' by udev.");
PARAM(update,           0,              "Triggers an update of the acceleration parameters below");
PARAM_F(PreScaleX,      PRE_SCALE_X,    "Prescale X-Axis before applying acceleration.");
PARAM_F(PreScaleY,      PRE_SCALE_Y,    "Prescale Y-Axis before applying acceleration.");
PARAM_F(SpeedCap,       SPEED_CAP,      "Limit the maximum pointer speed before applying acceleration.");
PARAM_F(Sensitivity,    SENSITIVITY,    "Mouse base sensitivity.");
PARAM_F(Acceleration,   ACCELERATION,   "Mouse acceleration sensitivity.");
PARAM_F(SensitivityCap, SENS_CAP,       "Cap maximum sensitivity.");
PARAM_F(Offset,         OFFSET,         "Mouse base sensitivity.");
PARAM_F(PostScaleX,     POST_SCALE_X,   "Postscale X-Axis after applying acceleration.");
PARAM_F(PostScaleY,     POST_SCALE_Y,   "Postscale >-Axis after applying acceleration.");
PARAM_F(ScrollsPerTick, SCROLLS_PER_TICK,"Amount of lines to scroll per scroll-wheel tick.");
#define PARAM_UPDATE(param) atof(g_param_##param, strlen(g_param_##param) , &g_##param);

static ktime_t g_next_update = 0;
INLINE void updata_params(ktime_t now)
{
    if(!g_update) return;
    if(now < g_next_update) return;
    g_update = 0;
    g_next_update = now + 1000000000ll;
    PARAM_UPDATE(PreScaleX);
    PARAM_UPDATE(PreScaleY);
    PARAM_UPDATE(SpeedCap);
    PARAM_UPDATE(Sensitivity);
    PARAM_UPDATE(Acceleration);
    PARAM_UPDATE(SensitivityCap);
    PARAM_UPDATE(Offset);
    PARAM_UPDATE(PostScaleX);
    PARAM_UPDATE(PostScaleY);
    PARAM_UPDATE(ScrollsPerTick);
}

int accelerate(int *x, int *y, int *wheel)
{
	float delta_x, delta_y, delta_whl, ms, rate, accel_sens;
    static long buffer_x = 0;
    static long buffer_y = 0;
    static long buffer_whl = 0;
	static float carry_x = 0.0f;
    static float carry_y = 0.0f;
    static float carry_whl = 0.0f;
	static float last_ms = 1.0f;
	static ktime_t last;
	ktime_t now;
    int status = 0;

    if(!irq_fpu_usable()){
        buffer_x += *x;
        buffer_y += *y;
        buffer_whl += *wheel;
        return -EBUSY;
    }

kernel_fpu_begin();
    accel_sens = g_Sensitivity;

    delta_x = (float) (*x);
    delta_y = (float) (*y);
    delta_whl = (float) (*wheel);

    if(!((int) delta_x == *x && (int) delta_y == *y && (int) delta_whl == *wheel)){
        buffer_x += *x;
        buffer_y += *y;
        buffer_whl += *wheel;
        status = -EFAULT;
        printk("MOUSE_ACCELERATION: First float-trap triggered. Should very very rarely happen, if at all");
        goto exit;
    }
    delta_x += (float) buffer_x; buffer_x = 0;
    delta_y += (float) buffer_y; buffer_y = 0;
    delta_whl += (float) buffer_whl; buffer_whl = 0;
    now = ktime_get();
    ms = (now - last)/(1000*1000);
    last = now;
    if(ms < 1) ms = last_ms;    
    if(ms > 100) ms = 100;
    last_ms = ms;
    updata_params(now);
    delta_x *= g_PreScaleX;
    delta_y *= g_PreScaleY;
    rate = delta_x * delta_x + delta_y * delta_y;
    B_sqrt(&rate);
    if(g_SpeedCap != 0){
        if (rate >= g_SpeedCap) {
            delta_x *= g_SpeedCap / rate;
            delta_y *= g_SpeedCap / rate;
            rate = g_SpeedCap;
        }
    }
    rate /= ms;
    rate -= g_Offset;
    if(rate > 0){
        rate *= g_Acceleration;
        accel_sens += rate;
    }
    if(g_SensitivityCap > 0 && accel_sens >= g_SensitivityCap){
        accel_sens = g_SensitivityCap;
    }
    accel_sens /= g_Sensitivity;
    delta_x *= accel_sens;
    delta_y *= accel_sens;
    delta_x *= g_PostScaleX;
    delta_y *= g_PostScaleY;
    delta_whl *= g_ScrollsPerTick/3.0f;
    delta_x += carry_x;
    delta_y += carry_y;
    if((delta_whl < 0 && carry_whl < 0) || (delta_whl > 0 && carry_whl > 0))
        delta_whl += carry_whl;
    if(!(isfinite(&delta_x) && isfinite(&delta_y) && isfinite(&delta_whl))){
        buffer_x += *x;
        buffer_y += *y;
        buffer_whl += *wheel;
        printk("MOUSE_ACCELERATION: Acceleration of NaN value");
        status = -EFAULT;
        goto exit;
    }
    *x = Leet_round(&delta_x);
    *y = Leet_round(&delta_y);
    *wheel = Leet_round(&delta_whl);
    if(*x == -2147483648 || *y == -2147483648 || *wheel == -2147483648){
        printk("MOUSE_ACCELERATION: Final float-trap triggered. This should NEVER happen!");
        status = -EFAULT;
        goto exit;
    }
    carry_x = delta_x - *x;
    carry_y = delta_y - *y;
    carry_whl = delta_whl - *wheel;
    
exit:
kernel_fpu_end();

    return status;
}
