#include <linux/module.h>
#include <linux/kernel.h>
#include <linux/init.h>
#include <linux/init_task.h>
#include <linux/vmalloc.h>
#include <linux/proc_fs.h>
#include <asm/uaccess.h>

MODULE_LICENSE("GPL");
MODULE_AUTHOR("Alexey Zolotukhin");
MODULE_DESCRIPTION("Fortune Cookie Kernel Module");

#define COOKIE_BUF_SIZE PAGE_SIZE

#define F_READ_WRITE_FILE_NAME "fortune"
#define F_SYMLINK_NAME "fortune_symlink"
#define F_DIR_NAME "fortune_dir"

static struct proc_dir_entry *fortune_file;
static struct proc_dir_entry *fortune_dir;
static struct proc_dir_entry *fortune_symlink;

static char *cookie_pot;
static unsigned next_fortune;
static unsigned cookie_index;

static char temp_buf[256];

static ssize_t fortune_write(struct file *, const char __user *, size_t, loff_t *);
static ssize_t fortune_read(struct file *, char __user *, size_t, loff_t *);
int fortune_open(struct inode *, struct file *);
int fortune_release(struct inode *, struct file *);

static struct proc_ops fops = 
{
    .proc_read = fortune_read,
    .proc_write = fortune_write,
    .proc_open = fortune_open,
    .proc_release = fortune_release,
};

int fortune_open(struct inode *sp_inode, struct file *sp_file)
{
    printk(KERN_INFO "+ fortune_open called\n");
    return 0;
}

int fortune_release(struct inode *sp_node, struct file *sp_file)
{
    printk(KERN_INFO "+ fortune_release called\n");
    return 0;
}

static ssize_t fortune_write(struct file *file, const char __user *buf, size_t count, loff_t *f_pos)
{
    int space_available = (COOKIE_BUF_SIZE - cookie_index) + 1;
    if (space_available < count)
    {
        printk(KERN_INFO "+ Error not enough space\n");
        return -ENOSPC;
    }
    if (copy_from_user(&cookie_pot[cookie_index], buf, count))
    {
        printk(KERN_INFO "+ Erorr copy_from_user\n");
        return -EFAULT;
    }
    cookie_index += count;
    cookie_pot[cookie_index - 1] = 0;
    printk(KERN_INFO "+ write finished\n");
    return count;
}

static ssize_t fortune_read(struct file *file, char __user *buf, size_t count, loff_t *f_pos)
{
    int len = 0;
	if (!cookie_index)
        return 0;
    if (*f_pos > 0)
        return 0;
    if (next_fortune >= cookie_index)
        next_fortune = 0;
    len = sprintf(temp_buf, "%s\n", &cookie_pot[next_fortune]);
    if (copy_to_user(buf, temp_buf, len))
    {
        printk(KERN_INFO "+ Error copy_to_user\n");
        return -EFAULT;
    }
    next_fortune += len;
    *f_pos += len;
	buf += len;
    printk(KERN_INFO "+ read finished\n");
    return len;
}

static void fortune_free(void) 
{
    if (fortune_symlink)
    {
        remove_proc_entry(F_SYMLINK_NAME, NULL);
        printk(KERN_INFO "+ Delete symlink " F_SYMLINK_NAME "\n");
    }
    if (fortune_file)
    {
        remove_proc_entry(F_READ_WRITE_FILE_NAME, NULL);
        printk(KERN_INFO "+ Delete file " F_READ_WRITE_FILE_NAME "\n");
    }
    if (fortune_dir)
    {
        remove_proc_entry(F_DIR_NAME, NULL);
        printk(KERN_INFO "+ Delete directory " F_DIR_NAME "\n");
    }
    if (cookie_pot)
        vfree(cookie_pot);
}

static int __init fortune_init(void)
{
    cookie_pot = (char *) vmalloc(COOKIE_BUF_SIZE);
    if (!cookie_pot)
    {
        printk(KERN_INFO "+ Error vmalloc\n");
        return -ENOMEM; 
    }
    memset(cookie_pot, 0, COOKIE_BUF_SIZE);
    if (!(fortune_file = proc_create(F_READ_WRITE_FILE_NAME, 0666, NULL, &fops)))
    {
        printk(KERN_INFO "+ Error proc_create fortune file\n");
        fortune_free();
        return -ENOMEM;
    }
    printk(KERN_INFO "+ Create file " F_READ_WRITE_FILE_NAME "\n");
    next_fortune = 0;
    cookie_index = 0;
    if (!(fortune_dir = proc_mkdir(F_DIR_NAME, NULL)))
    {
        printk(KERN_INFO "+ Error proc_mkdir\n");
        fortune_free();
        return -ENOMEM;
    }
    printk(KERN_INFO "+ Create directory " F_DIR_NAME "\n");
    if (!(fortune_symlink = proc_symlink(F_SYMLINK_NAME, NULL, "/proc/"F_DIR_NAME)))
    {
        printk(KERN_INFO "+ Error proc_symlink\n");
        fortune_free();
        return -ENOMEM;
    }
    printk(KERN_INFO "+ Create symlink " F_SYMLINK_NAME "\n");
    printk(KERN_INFO "+ Module init\n");
    return 0;
}

static void __exit fortune_exit(void)
{
    fortune_free();
    printk(KERN_INFO "+ Module exit\n");
}

module_init(fortune_init);
module_exit(fortune_exit);

