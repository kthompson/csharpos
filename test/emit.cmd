@echo off
gcc --omit-frame-pointer -S ctest.c
TYPE ctest.s
del ctest.s