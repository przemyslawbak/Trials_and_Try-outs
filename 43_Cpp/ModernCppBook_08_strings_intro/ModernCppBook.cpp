#include <iostream>

int main()
{
	std::string s = "Hello.";
	std::cout << s;

	s += "World.";
	std::cout << s;

	char c = '!';
	s += c;
	std::cout << s;

	std::string s1 = "Hello ";
	std::string s2 = "World.";
	std::string s3 = s1 + s2;
	std::cout << s3;

	char c1 = s[0]; // 'H'
	char c2 = s.at(0); // 'H';
	char c3 = s[6]; // 'W'

	std::string mysubstring = s.substr(6, 5);
	std::cout << "The substring value is: " << mysubstring;

	std::string stringtofind = "Hello";
	std::string::size_type found = s.find(stringtofind);
	if (found != std::string::npos)
	{
		std::cout << "Substring found at position: " << found;
	}
	else
	{
		std::cout << "The substring is not found.";
	}
}