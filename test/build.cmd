@echo off
gcc -Wall ctest.c runtime.c -o test
test
del test.exe