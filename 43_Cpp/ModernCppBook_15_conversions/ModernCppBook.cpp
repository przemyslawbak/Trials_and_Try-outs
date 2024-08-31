#include <iostream>

int main()
{
	char mychar = 64;
	int myint = 123;
	double mydouble = 456.789;
	bool myboolean = true;
	myint = mychar;
	mydouble = myint;
	mychar = myboolean;

	myint = mydouble; // the decimal part is lost

	char c1 = 10;
	char c2 = 20;
	auto result = c1 + c2; // result is of type int

	auto myinteger = static_cast<int>(123.456); //explicit
}
