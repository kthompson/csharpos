	.file	"ctest.c"
	.text
.globl _test_entryInt64
	.def	_test_entryInt64;	.scl	2;	.type	32;	.endef
_test_entryInt64:
	pushl	%ebp
	movl	%esp, %ebp
	movl	16(%ebp), %eax
	movl	20(%ebp), %edx
	addl	8(%ebp), %eax
	adcl	12(%ebp), %edx
	popl	%ebp
	ret
.globl _test_entryInt32
	.def	_test_entryInt32;	.scl	2;	.type	32;	.endef
_test_entryInt32:
	pushl	%ebp
	movl	%esp, %ebp
	movl	12(%ebp), %eax
	addl	8(%ebp), %eax
	popl	%ebp
	ret
.globl _test_entryInt
	.def	_test_entryInt;	.scl	2;	.type	32;	.endef
_test_entryInt:
	pushl	%ebp
	movl	%esp, %ebp
	subl	$4, %esp
	movl	8(%ebp), %eax
	movl	12(%ebp), %edx
	movw	%ax, -2(%ebp)
	movw	%dx, -4(%ebp)
	movzwl	-2(%ebp), %edx
	movl	-4(%ebp), %eax
	leal	(%edx,%eax), %eax
	cwtl
	leave
	ret
.globl _test_entryChar
	.def	_test_entryChar;	.scl	2;	.type	32;	.endef
_test_entryChar:
	pushl	%ebp
	movl	%esp, %ebp
	subl	$4, %esp
	movl	8(%ebp), %eax
	movl	12(%ebp), %edx
	movb	%al, -1(%ebp)
	movb	%dl, -2(%ebp)
	movzbl	-2(%ebp), %eax
	addb	-1(%ebp), %al
	movsbl	%al,%eax
	leave
	ret
.globl _test_retInt64
	.def	_test_retInt64;	.scl	2;	.type	32;	.endef
_test_retInt64:
	pushl	%ebp
	movl	%esp, %ebp
	
	movl	8(%ebp), %eax
	movl	12(%ebp), %edx
	
	popl	%ebp
	ret
.globl _test_retInt32
	.def	_test_retInt32;	.scl	2;	.type	32;	.endef
_test_retInt32:
	pushl	%ebp
	movl	%esp, %ebp
	
	movl	8(%ebp), %eax
	
	popl	%ebp
	ret
.globl _test_retInt
	.def	_test_retInt;	.scl	2;	.type	32;	.endef
_test_retInt:
	pushl	%ebp
	movl	%esp, %ebp
	
	subl	$4, %esp
	
	movl	8(%ebp), %eax
	movw	%ax, -2(%ebp)
	movswl	-2(%ebp),%eax
	
	leave
	ret
.globl _test_retChar
	.def	_test_retChar;	.scl	2;	.type	32;	.endef
_test_retChar:
	pushl	%ebp
	movl	%esp, %ebp
	
	subl	$4, %esp
	
	movl	8(%ebp), %eax
	movb	%al, -1(%ebp)
	movsbl	-1(%ebp),%eax
	
	leave
	ret
.globl _test_entryInt64V
	.def	_test_entryInt64V;	.scl	2;	.type	32;	.endef
_test_entryInt64V:
	pushl	%ebp
	movl	%esp, %ebp
	
	subl	$8, %esp
	
	movl	16(%ebp), %eax
	movl	20(%ebp), %edx
	addl	8(%ebp), %eax
	adcl	12(%ebp), %edx
	movl	%eax, -8(%ebp)
	movl	%edx, -4(%ebp)
	
	leave
	ret
.globl _test_entryInt32V
	.def	_test_entryInt32V;	.scl	2;	.type	32;	.endef
_test_entryInt32V:
	pushl	%ebp
	movl	%esp, %ebp
	
	subl	$4, %esp
	
	movl	12(%ebp), %eax
	addl	8(%ebp), %eax
	movl	%eax, -4(%ebp)
	
	leave
	ret
.globl _test_entryIntV
	.def	_test_entryIntV;	.scl	2;	.type	32;	.endef
_test_entryIntV:
	pushl	%ebp
	movl	%esp, %ebp
	
	subl	$8, %esp
	
	movl	8(%ebp), %eax
	movl	12(%ebp), %edx
	movw	%ax, -2(%ebp)
	movw	%dx, -4(%ebp)
	movzwl	-2(%ebp), %edx
	movl	-4(%ebp), %eax
	leal	(%edx,%eax), %eax
	movw	%ax, -6(%ebp)
	
	leave
	ret
.globl _test_entryCharV
	.def	_test_entryCharV;	.scl	2;	.type	32;	.endef
_test_entryCharV:
	pushl	%ebp
	movl	%esp, %ebp
	
	subl	$4, %esp
	
	movl	8(%ebp), %eax
	movl	12(%ebp), %edx
	movb	%al, -1(%ebp)
	movb	%dl, -2(%ebp)
	movzbl	-2(%ebp), %eax
	addb	-1(%ebp), %al
	movb	%al, -3(%ebp)
	leave
	ret
