#include <iostream>

int main()
{
	try
	{
		std::cout << "Let's assume some error occurred in our program." << '\n';
		std::cout << "We throw an exception of type int, for example." << '\n';
		std::cout << "This signals that something went wrong." << '\n';
		throw std::string{ "Some string error" }; // throw an exception // if there is an error
	}
	catch (const std::string& e)
	{
		// catch and handle the exception
		std::cout << "String exception raised!" << '\n';
		std::cout << "The exception has a value of: " << e << '\n';
	}

	try
	{
		bool someflag = true;
		bool someotherflag = true;
		std::cout << "We can have multiple throw exceptions." << '\n';
		if (someflag)
		{
			std::cout << "Throwing an int exception." << '\n';
			throw 123;
		}
		if (someotherflag)
		{
			std::cout << "Throwing a string exception." << '\n';
			throw std::string{ "Some string error" };
		}
	}
	catch (int e)
	{
		// catch and handle the exception
		std::cout << "Integer exception raised!." << '\n';
		std::cout << "The exception has a value of: " << e << '\n';
	}
	catch (const std::string& e)
	{
		// catch and handle the exception
		std::cout << "String exception raised!" << '\n';
		std::cout << "The exception has a value of: " << e << '\n';
	}
}
