@echo off
gcc --omit-frame-pointer -S ctest.c
TYPE ctest.s | find /v ".globl" | find /v ".def"
del ctest.s