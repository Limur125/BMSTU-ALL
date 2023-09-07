#include <linux/interrupt.h>
#include <linux/kernel.h>
#include <linux/module.h>
#include <linux/proc_fs.h>
#include <linux/workqueue.h>
#include <linux/delay.h>
#include <linux/jiffies.h>
#include <asm/io.h>

MODULE_LICENSE("GPL");
MODULE_AUTHOR("Zolotukhin Alexey");

#define IRQ_NUM 1

struct workqueue_struct *workqueue;
int code;
struct work_struct work1, work2;

const unsigned char kbdus[128] =
{
    0,  27, '1', '2', '3', '4', '5', '6', '7', '8',	/* 9 */
  '9', '0', '-', '=', '\b',	/* Backspace */
  '\t',			/* Tab */
  'q', 'w', 'e', 'r',	/* 19 */
  't', 'y', 'u', 'i', 'o', 'p', '[', ']', '\n',	/* Enter key */
    0,			/* 29   - Control */
  'a', 's', 'd', 'f', 'g', 'h', 'j', 'k', 'l', ';',	/* 39 */
 '\'', '`',   0,		/* Left shift */
 '\\', 'z', 'x', 'c', 'v', 'b', 'n',			/* 49 */
  'm', ',', '.', '/',   0,				/* Right shift */
  '*',
    0,	/* Alt */
  ' ',	/* Space bar */
    0,	/* Caps lock */
    0,	/* 59 - F1 key ... > */
    0,   0,   0,   0,   0,   0,   0,   0,
    0,	/* < ... F10 */
    0,	/* 69 - Num lock*/
    0,	/* Scroll Lock */
    0,	/* Home key */
    0,	/* Up Arrow */
    0,	/* Page Up */
  '-',
    0,	/* Left Arrow */
    0,
    0,	/* Right Arrow */
  '+',
    0,	/* 79 - End key*/
    0,	/* Down Arrow */
    0,	/* Page Down */
    0,	/* Insert Key */
    0,	/* Delete Key */
    0,   0,   0,
    0,	/* F11 Key */
    0,	/* F12 Key */
    0,	/* All other keys are undefined */
};
void queue_function1(struct work_struct *work)
{
    printk(KERN_INFO "workqueue_mod: work1 keyboard=%c, time=%llu\n", kbdus[code & 0x7f], ktime_get());
}

void queue_function2(struct work_struct *work)
{
	unsigned long start, delay_time;

	printk(KERN_INFO "workqueue_mod: work2 stop time=%llu\n", ktime_get());
	start = jiffies;
	msleep(1000);
	delay_time = jiffies - start;
	printk(KERN_INFO "workqueue_mod: work2 continue time=%llu\n", ktime_get());
	printk(KERN_INFO "workqueue_mod: work2 sleep for %u miliseconds", jiffies_to_msecs(delay_time));
    printk(KERN_INFO "workqueue_mod: work2 keyboard=%c, time=%llu\n\n", kbdus[code & 0x7f], ktime_get());
}

irqreturn_t handler(int irq, void *dev)
{
	code = inb(0x60);
	
    if (irq == IRQ_NUM)
    {
        queue_work(workqueue, &work1);
        queue_work(workqueue, &work2);
        return IRQ_HANDLED;
    }
    return IRQ_NONE;
}


static int __init work_queue_init(void)
{
    int ret;
    ret = request_irq(IRQ_NUM, handler, IRQF_SHARED, "my_dev", &handler);
	
    if (ret)
    {
        printk(KERN_ERR "workqueue_mod: request_irq() failed!\n");
        return ret;
    }

    if (!(workqueue = alloc_workqueue("my_queue", WQ_FREEZABLE, 1)))
    {
        free_irq(IRQ_NUM, &handler);
        printk(KERN_INFO "workqueue_mod: create_workqueue() failed!");
        return -ENOMEM;
    }

    INIT_WORK(&work1, queue_function1);
    INIT_WORK(&work2, queue_function2);

    printk(KERN_INFO "workqueue_mod: module loaded!\n");
    return 0;
}

static void __exit work_queue_exit(void)
{
    flush_workqueue(workqueue);
    destroy_workqueue(workqueue);
    free_irq(IRQ_NUM, &handler);
    kfree(&work1);
    kfree(&work2);
    printk(KERN_INFO "workqueue_mod: module unloaded!\n");
}

module_init(work_queue_init) 
module_exit(work_queue_exit)
