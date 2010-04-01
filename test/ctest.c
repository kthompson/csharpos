#include <stdio.h>

__int64 test_entryInt64(__int64 a, __int64 b){
	return a+b; 
}

long test_entryInt32(long a, long b){
	return a+b; 
}

short test_entryInt16(short a, short b){
	return a+b; 
}

char test_entryChar(char a, char b){
	return a+b; 
}

__int64 test_retInt64(__int64 a){
	return a; 
}

long test_retInt32(long a){
	return a; 
}

short test_retInt16(short a){
	return a; 
}

char test_retChar(char a){
	return a; 
}

void test_entryInt64V(__int64 a, __int64 b){
	__int64 c = a+b; 
}

void test_entryInt32V(long a, long b){
	long c = a+b; 
}

void test_entryInt16V(short a, short b){
	short c = a+b; 
}

void test_entryCharV(char a, char b){
	char c = a+b; 
}
