#include <iostream>

int main()
{
	int x = 123;
	int* p = &x;

	char* po = nullptr; // points to nothing

	std::cout << "The value of the dereferenced pointer is: " << *p;

	*p = 456; // change the value of pointed-to object
	std::cout << "The value of x is: " << x;
}