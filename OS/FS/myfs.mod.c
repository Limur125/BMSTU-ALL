#include <linux/module.h>
#define INCLUDE_VERMAGIC
#include <linux/build-salt.h>
#include <linux/elfnote-lto.h>
#include <linux/export-internal.h>
#include <linux/vermagic.h>
#include <linux/compiler.h>

BUILD_SALT;
BUILD_LTO_INFO;

MODULE_INFO(vermagic, VERMAGIC_STRING);
MODULE_INFO(name, KBUILD_MODNAME);

__visible struct module __this_module
__section(".gnu.linkonce.this_module") = {
	.name = KBUILD_MODNAME,
	.init = init_module,
#ifdef CONFIG_MODULE_UNLOAD
	.exit = cleanup_module,
#endif
	.arch = MODULE_ARCH_INIT,
};

#ifdef CONFIG_RETPOLINE
MODULE_INFO(retpoline, "Y");
#endif


static const struct modversion_info ____versions[]
__used __section("__versions") = {
	{ 0xbdfb6dbb, "__fentry__" },
	{ 0x5b8239ca, "__x86_return_thunk" },
	{ 0x92997ed8, "_printk" },
	{ 0xe0280b67, "new_inode" },
	{ 0x6e672ee6, "current_time" },
	{ 0xd0da656b, "__stack_chk_fail" },
	{ 0xf7be14e9, "mount_bdev" },
	{ 0x88db9f48, "__check_object_size" },
	{ 0x13c49cc2, "_copy_from_user" },
	{ 0xb742fd7, "simple_strtol" },
	{ 0x656e4a6e, "snprintf" },
	{ 0x6b10bee1, "_copy_to_user" },
	{ 0x7a6aa7e1, "d_alloc_name" },
	{ 0x794f41bf, "d_add" },
	{ 0x8612eacf, "dput" },
	{ 0xb28523ef, "simple_dir_operations" },
	{ 0x1ff731c2, "init_user_ns" },
	{ 0xc727fcc2, "inode_init_owner" },
	{ 0x38699e1b, "simple_dir_inode_operations" },
	{ 0x71cbab8b, "d_make_root" },
	{ 0x8639096a, "iput" },
	{ 0x5f540977, "kmalloc_caches" },
	{ 0xfa55b3ee, "kmem_cache_alloc_trace" },
	{ 0xbaba6804, "kmem_cache_create" },
	{ 0x37a0cba, "kfree" },
	{ 0x1705c1ec, "kmem_cache_alloc" },
	{ 0xe14703cb, "kmem_cache_free" },
	{ 0x789d652, "kmem_cache_destroy" },
	{ 0x20a4e878, "register_filesystem" },
	{ 0xac30ae97, "unregister_filesystem" },
	{ 0xd95910a8, "kill_block_super" },
	{ 0xb7aa7543, "generic_delete_inode" },
	{ 0x1e3f1b95, "simple_statfs" },
	{ 0x541a6db8, "module_layout" },
};

MODULE_INFO(depends, "");


MODULE_INFO(srcversion, "389D8585C8F724FA4982746");
