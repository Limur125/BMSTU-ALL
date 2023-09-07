#include <linux/init.h>
#include <linux/module.h>

#include "md.h"

MODULE_LICENSE("GPL");
MODULE_AUTHOR("Alexey Zolotukhin");


static int __init md_init(void)
{
	printk("+ module md3 start!\n");
	printk("+ data string exported from md1 : %s\n", md1_data);
	printk("+ string returned md1_proc() is : %s\n", md1_proc());
	

	//printk("+ string returned md1_noexport() is : %s\n", md1_noexport()); //1

	//printk("+ string returned md1_local() is : %s\n", md1_local()); //2
	return 0;
}

module_init(md_init);
