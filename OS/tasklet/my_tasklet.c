#include <linux/interrupt.h>
#include <linux/module.h>
#include <linux/slab.h>
#include <linux/init.h>
#include <linux/sched.h>
#include <linux/time.h>

#define IRQ_NUM 1

MODULE_LICENSE("GPL");
MODULE_AUTHOR("Aleksey Zolotukhin");

char *my_tasklet_data = "my_tasklet data";
struct tasklet_struct* my_tasklet;


void my_tasklet_function(unsigned long data)
{
    printk( ">> my_tasklet: Bottom-half handled time=%llu\n", ktime_get());
}

irqreturn_t my_handler(int irq, void *dev)
{
    printk(">> my_tasklet: Top-half start time=%llu\n", ktime_get());
    if (irq == IRQ_NUM)
    {
        tasklet_schedule(my_tasklet);
		printk(">> my_tasklet: Bottom-half sheduled time=%llu\n", ktime_get());
        return IRQ_HANDLED;
    }
    printk(">> my_tasklet: irq wasn't handled\n");
    return IRQ_NONE;
}

static int __init my_init(void)
{
    my_tasklet = kmalloc(sizeof(struct tasklet_struct), GFP_KERNEL); 
    if (!my_tasklet)
    {
        printk(">> my_tasklet: ERROR kmalloc!\n");
        return -1;
    }

    tasklet_init(my_tasklet, my_tasklet_function, (unsigned long)my_tasklet_data);

    if (request_irq(IRQ_NUM, my_handler, IRQF_SHARED, "my_dev_name", &my_handler))
    {
        printk(">> my_tasklet: ERROR request_irq\n");
        return -1;
    }
    printk(">> my_tasklet: module loaded\n");
    return 0;
}

static void __exit my_exit(void)
{
    tasklet_kill(my_tasklet);
    kfree(my_tasklet);
    free_irq(IRQ_NUM, &my_handler);
    printk(">> my_tasklet: " "module unloaded\n");
}

module_init(my_init) 
module_exit(my_exit)


