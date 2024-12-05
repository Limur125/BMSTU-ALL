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
	{ 0x7b0c27e0, "usb_kill_urb" },
	{ 0xca4d9c38, "input_unregister_device" },
	{ 0xef07e8e1, "usb_free_urb" },
	{ 0xb041f92d, "usb_free_coherent" },
	{ 0x37a0cba, "kfree" },
	{ 0x2e6c9f5a, "usb_submit_urb" },
	{ 0x5f0caf0d, "_dev_err" },
	{ 0x122c3a7e, "_printk" },
	{ 0xdb8c68f, "input_event" },
	{ 0x21d81531, "usb_deregister" },
	{ 0x7affd727, "kmalloc_caches" },
	{ 0x3d650a07, "kmalloc_trace" },
	{ 0xa626c9ce, "input_allocate_device" },
	{ 0xff12de57, "usb_alloc_coherent" },
	{ 0xef31351c, "usb_alloc_urb" },
	{ 0x754d539c, "strlen" },
	{ 0xf9c0b663, "strlcat" },
	{ 0xa916b694, "strnlen" },
	{ 0x656e4a6e, "snprintf" },
	{ 0x170700e2, "input_register_device" },
	{ 0x89a426a9, "input_free_device" },
	{ 0xcbd4898c, "fortify_panic" },
	{ 0xbdfb6dbb, "__fentry__" },
	{ 0x914ccc00, "usb_register_driver" },
	{ 0x5b8239ca, "__x86_return_thunk" },
	{ 0x453e7dc, "module_layout" },
};

MODULE_INFO(depends, "");

MODULE_ALIAS("usb:v*p*d*dc*dsc*dp*ic03isc01ip02in*");

MODULE_INFO(srcversion, "86785DA749F90C9F0A2186F");
