@echo off
gcc --omit-frame-pointer -S -O0 -fverbose-asm ctest.c
TYPE ctest.s | find /v ".globl" | find /v ".def"
del ctest.s