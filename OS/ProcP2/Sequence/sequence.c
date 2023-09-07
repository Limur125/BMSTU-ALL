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
int fortune_show(struct seq_file *, void *);
int fortune_open(struct inode *, struct file *);
int fortune_release(struct inode *, struct file *);

static struct proc_ops fops = 
{
    .proc_read = seq_read,
    .proc_write = fortune_write,
    .proc_open = fortune_open,
    .proc_release = seq_release,
};

void* sequence_start(struct seq_file *m, loff_t *pos)
{
	printk(KERN_INFO "+ seqeunce_start()");
	static unsigned long counter = 0;
	if (*pos == 0) 
	{
		return &counter;
	}
	else
	{
		*pos = 0;
		return NULL;
	}
}

void sequence_stop(struct seq_file *m, void *v)
{
	printk(KERN_INFO "+ sequence_stop()");
	if (v == NULL)
		printk(KERN_INFO "+ v pointer NULL");
	else 
		printk(KERN_INFO "+ v pointer %p", v);
}

void* sequence_next(struct seq_file *m, void* v, loff_t *pos)
{
	printk(KERN_INFO "+ sequence_next()");
	(*pos)++;
	return NULL;
}

int sequence_show(struct seq_file* m, void* v) 
{
    int len;
    if (!cookie_index)
        return 0;
    if (next_fortune >= cookie_index)
        next_fortune = 0;
    len = sprintf(temp_buf, "%s", &cookie_pot[next_fortune]);
	seq_printf(m, "%s", temp_buf);
    next_fortune += len + 1;
    printk(KERN_INFO "+ read finished\n");
    return 0;
}

static struct seq_operations fortune_seq_ops = {
  .start = sequence_start,
  .next = sequence_next,
  .stop = sequence_stop,
  .show = sequence_show
};

int fortune_open(struct inode *sp_inode, struct file *sp_file)
{
    printk(KERN_INFO "+ fortune_open called\n");
    return seq_open(sp_file, &fortune_seq_ops);
}

int fortune_release(struct inode *sp_node, struct file *sp_file)
{
    printk(KERN_INFO "+ fortune_release called\n");
    return single_release(sp_node, sp_file);
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
