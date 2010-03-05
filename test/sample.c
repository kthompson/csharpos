#include <stdio.h>

__int64 test_entryInt64(__int64 a, __int64 b);
long test_entryInt32(long a, long b);
short test_entryInt(short a, short b);
char test_entryChar(char a, char b);

__int64 test_retInt64(__int64 a);
long test_retInt32(long a);
short test_retInt(short a);
char test_retChar(char a);

void test_entryInt64V(__int64 a, __int64 b);
void test_entryInt32V(long a, long b);
void test_entryIntV(short a, short b);
void test_entryCharV(char a, char b);


int main(int argc, char** argv){
	
	__int64 a = test_entryInt64(1, 2);
	long b = test_entryInt32(1, 2);
	short c = test_entryInt(1, 2);
	char d = test_entryChar(1, 2);

	__int64 e = test_retInt64(123);
	long f = test_retInt32(123);
	short g = test_retInt(123);
	char h = test_retChar(123);

	test_entryInt64V(1, 2);
	test_entryInt32V(1, 2);
	test_entryIntV(1, 2);
	test_entryCharV(1, 2);
	
	return 0;
}
