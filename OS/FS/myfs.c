#include <linux/fs.h>
#include <linux/init.h>
#include <linux/kernel.h>
#include <linux/module.h>
#include <linux/slab.h>
#include <linux/time.h>

#define MYFS_MAGIC_NUMBER 0x13131313
#define MAX_CACHE_SIZE 128
#define SLAB_NAME "myfs_slab"
#define TMPSIZE 100

MODULE_LICENSE("GPL");
MODULE_AUTHOR("Alexey Zolotukhin");

static int sco = 0;
static int number = 31;
static atomic_t counter, subcounter; 
static int **line = NULL;
static int size = sizeof(struct inode);
static struct kmem_cache *cache = NULL; 

static struct inode *myfs_make_inode(struct super_block *sb, int mode, const struct file_operations* fops) 
{
    struct inode *ret = new_inode(sb);
	if (ret)
	{
        ret->i_mode = mode;
		ret->i_ino = number++;
		ret->i_size = PAGE_SIZE;
		ret->i_atime = ret->i_mtime = ret->i_ctime = current_time(ret);
        ret->i_fop = fops;
		ret->i_private = &ret;
	}
	return ret;
}

static int myfs_open(struct inode *inode, struct file *filp)
{
	filp->private_data = inode->i_private;
	return 0;
}

static ssize_t myfs_read_file(struct file *filp, char *buf, size_t count, loff_t *offset)
{
	atomic_t *counter = (atomic_t *) filp->private_data;
	int v, len;
	char tmp[TMPSIZE];
	v = atomic_read(counter);
	if (*offset > 0)
		v -= 1;
	else
		atomic_inc(counter);
	len = snprintf(tmp, TMPSIZE, "%d\n", v);
	if (*offset > len)
		return 0;
	if (count > len - *offset)
		count = len - *offset;
	if (copy_to_user(buf, tmp + *offset, count))
		return -EFAULT;
	*offset += count;
	return count;
}

static ssize_t myfs_write_file(struct file *filp, const char *buf, size_t count, loff_t *offset)
{
	atomic_t *counter = (atomic_t *) filp->private_data;
	char tmp[TMPSIZE];
	if (*offset != 0)
		return -EINVAL;
	if (count >= TMPSIZE)
		return -EINVAL;
	memset(tmp, 0, TMPSIZE);
	if (copy_from_user(tmp, buf, count))
		return -EFAULT;
	atomic_set(counter, simple_strtol(tmp, NULL, 10));
	return count;
}

static struct file_operations myfs_file_ops = {
	.open	= myfs_open,
	.read 	= myfs_read_file,
	.write  = myfs_write_file,
};

static struct dentry *myfs_create_file (struct super_block *sb, struct dentry *dir, const char *name, atomic_t *counter)
{
	struct dentry *dentry;
	struct inode *inode;
	dentry = d_alloc_name(dir, name);
	if (!dentry)
	{
        printk(KERN_ERR "MYFS: dentry creation failed\n");
		return NULL;
    }
	inode = myfs_make_inode(sb, S_IFREG | 0644, &myfs_file_ops);
	if (!inode)
	{
        dput(dentry);
        printk(KERN_ERR "MYFS: inode creation failed\n");
        return NULL;
    }
	inode->i_private = counter;
	d_add(dentry, inode);
	return dentry;
}

static struct dentry *myfs_create_dir (struct super_block *sb, struct dentry *parent, const char *name)
{
	struct dentry *dentry;
	struct inode *inode;

	dentry = d_alloc_name(parent, name);
	if (!dentry)
    {
        printk(KERN_ERR "MYFS: dentry creation failed\n");
		return NULL;
    }
	inode = myfs_make_inode(sb, S_IFDIR | 0755, &simple_dir_operations);
	if (!inode)
	{
        dput(dentry);
        printk(KERN_ERR "MYFS: inode creation failed\n");
        return NULL;
    }
	inode->i_op = &simple_dir_inode_operations;

	d_add(dentry, inode);
	return dentry;
}

static void myfs_create_files (struct super_block *sb, struct dentry *root)
{
	struct dentry *subdir;
	atomic_set(&counter, 0);
	myfs_create_file(sb, root, "counter", &counter);
	atomic_set(&subcounter, 0);
	subdir = myfs_create_dir(sb, root, "subdir");
	if (subdir)
		myfs_create_file(sb, subdir, "subcounter", &subcounter);
}

static void myfs_put_super(struct super_block *sb) 
{
    printk(KERN_DEBUG "MYFS: super block destroyed\n");
}

static struct super_operations const myfs_super_ops = 
{
      .put_super = myfs_put_super,
      .statfs = simple_statfs,
      .drop_inode = generic_delete_inode,
};

static int myfs_fill_sb(struct super_block *sb, void *data, int silent) 
{
    struct inode *root = NULL; 
    sb->s_blocksize = PAGE_SIZE;
    sb->s_blocksize_bits = PAGE_SHIFT;
    sb->s_magic = MYFS_MAGIC_NUMBER;
    sb->s_op = &myfs_super_ops; 
    root = myfs_make_inode(sb, S_IFDIR | 0755, &simple_dir_operations);
    if (!root) 
    {
        printk(KERN_ERR "MYFS: inode allocation failed\n");
        return -ENOMEM;
    }
    inode_init_owner(&init_user_ns, root, NULL, S_IFDIR | 0755);
    root->i_op = &simple_dir_inode_operations;
    if (!(sb->s_root = d_make_root(root)))
    {
        printk(KERN_ERR "MYFS: root creation failed\n");
        iput(root);
        return -ENOMEM;
    }
    myfs_create_files(sb, sb->s_root);
    return 0;
}

static struct dentry *myfs_mount(struct file_system_type *type, int flags, char const *dev, void *data)
{
    struct dentry* const entry = mount_bdev(type, flags, dev, data, myfs_fill_sb);
    if (IS_ERR(entry))
        printk(KERN_ERR "MYFS: mounting failed\n");
    else
        printk(KERN_DEBUG "MYFS: mounted!\n");
    return entry;
}

static struct file_system_type myfs_type = 
{
    .owner = THIS_MODULE,
    .name = "myfs",
    .fs_flags = FS_REQUIRES_DEV,
    .mount = myfs_mount,
    .kill_sb = kill_block_super,
};

void co(void* p)
{ 
    *(int*)p = (int)p; 
    sco++; 
} 

static int __init myfs_init(void) 
{
	int ret;
	
	line = kmalloc(sizeof(void*), GFP_KERNEL); 
    if(!line)
    { 
        printk(KERN_ERR "MYFS: kmalloc error\n"); 
        return -ENOMEM;
    }
 
	cache = kmem_cache_create(SLAB_NAME, size, 0, 0, co); 
    if(!cache) 
    { 
        printk(KERN_ERR "MYFS: kmem_cache_create error\n"); 
        kfree(line); 
	    return -ENOMEM;  
    } 

    if (!((*line) = kmem_cache_alloc(cache, GFP_KERNEL)))
    {
        printk(KERN_ERR "MYFS: kmem_cache_alloc error\n"); 

        kmem_cache_free(cache, *line);
        kmem_cache_destroy(cache);
        kfree(line);

        return -ENOMEM;
    }

    ret = register_filesystem(&myfs_type);

    if (ret != 0) 
    {
        printk(KERN_ERR "MYFS: Failed to register filesystem\n");
        kmem_cache_destroy(cache); 
        kfree(line);
        return ret;
    }
    
    printk(KERN_INFO "MYFS: MYFS_MODULE loaded!\n");

    printk(KERN_INFO "MYFS: allocate %d objects into slab: %s\n", number, SLAB_NAME); 
	printk(KERN_INFO "MYFS: object size %d bytes, full size %ld bytes\n", size, (long)size * number); 
	printk(KERN_INFO "MYFS: constructor called %d times\n", sco); 

    return 0;
}


static void __exit myfs_exit(void) 
{
	kmem_cache_free(cache, *line);
    kmem_cache_destroy(cache); 
    kfree(line);
    
    if (unregister_filesystem(&myfs_type))
        printk(KERN_ERR "MYFS: MYFS_MODULE can not unregister filesystem!\n");

    printk(KERN_INFO "MYFS: MYFS_MODULE unloaded!\n");
}

module_init(myfs_init);
module_exit(myfs_exit);

