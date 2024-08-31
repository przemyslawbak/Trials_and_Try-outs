#include <iostream>
#include <fstream>
#include <sstream>

int main()
{
	std::fstream fs{ "myfile.txt" };
	char c;
	while (fs >> std::noskipws >> c)
	{
		std::cout << c;
	}

	char c = 'A';
	int x = 123;
	double d = 456.78;
	std::stringstream ss;
	ss << c << x << d;
	std::cout << ss.str();
}