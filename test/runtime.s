	.file	"runtime.c"
	.def	___main;	.scl	2;	.type	32;	.endef
	.text
.globl _main
	.def	_main;	.scl	2;	.type	32;	.endef
_main:
	pushl	%ebp
	movl	%esp, %ebp
	
		subl	$56, %esp
		
		andl	$-16, %esp
		movl	$0, %eax
		addl	$15, %eax
		addl	$15, %eax
		shrl	$4, %eax
		sall	$4, %eax
		movl	%eax, -36(%ebp)
		movl	-36(%ebp), %eax
		call	__alloca
		call	___main
		
		movl	$2, 8(%esp)
		movl	$0, 12(%esp)
		movl	$1, (%esp)
		movl	$0, 4(%esp)
		call	_test_entryInt64
		movl	%eax, -8(%ebp)
		movl	%edx, -4(%ebp)
		
		movl	$2, 4(%esp)
		movl	$1, (%esp)
		call	_test_entryInt32
		movl	%eax, -12(%ebp)
		
		movl	$2, 4(%esp)
		movl	$1, (%esp)
		call	_test_entryInt
		movw	%ax, -14(%ebp)
		
		movl	$2, 4(%esp)
		movl	$1, (%esp)
		call	_test_entryChar
		movb	%al, -15(%ebp)
		
		movl	$123, (%esp)
		movl	$0, 4(%esp)
		call	_test_retInt64
		movl	%eax, -24(%ebp)
		movl	%edx, -20(%ebp)
		
		movl	$123, (%esp)
		call	_test_retInt32
		movl	%eax, -28(%ebp)
		
		movl	$123, (%esp)
		call	_test_retInt
		movw	%ax, -30(%ebp)
		
		movl	$123, (%esp)
		call	_test_retChar
		movb	%al, -31(%ebp)
		
		movl	$2, 8(%esp)
		movl	$0, 12(%esp)
		movl	$1, (%esp)
		movl	$0, 4(%esp)
		call	_test_entryInt64V
		
		movl	$2, 4(%esp)
		movl	$1, (%esp)
		call	_test_entryInt32V
		
		movl	$2, 4(%esp)
		movl	$1, (%esp)
		call	_test_entryIntV
		
		movl	$2, 4(%esp)
		movl	$1, (%esp)
		call	_test_entryCharV
		
		movl	$0, %eax
	
	leave
	ret
	.def	_test_entryCharV;	.scl	2;	.type	32;	.endef
	.def	_test_entryIntV;	.scl	2;	.type	32;	.endef
	.def	_test_entryInt32V;	.scl	2;	.type	32;	.endef
	.def	_test_entryInt64V;	.scl	2;	.type	32;	.endef
	.def	_test_retChar;	.scl	2;	.type	32;	.endef
	.def	_test_retInt;	.scl	2;	.type	32;	.endef
	.def	_test_retInt32;	.scl	2;	.type	32;	.endef
	.def	_test_retInt64;	.scl	2;	.type	32;	.endef
	.def	_test_entryChar;	.scl	2;	.type	32;	.endef
	.def	_test_entryInt;	.scl	2;	.type	32;	.endef
	.def	_test_entryInt32;	.scl	2;	.type	32;	.endef
	.def	_test_entryInt64;	.scl	2;	.type	32;	.endef
