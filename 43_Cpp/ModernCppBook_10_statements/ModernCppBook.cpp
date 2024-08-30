#include <iostream>

int main()
{
	bool d = false;
	if (d) std::cout << "The condition is true.";
	else std::cout << "The condition is false.";

	bool mycondition = true;
	int x = 0;
	x = (mycondition) ? 1 : 0;
	std::cout << "The value of x is: " << x << '\n';

	bool a = true;
	bool b = true;
	if (a && b)
	{
		std::cout << "The entire condition is true.";
	}
	else
	{
		std::cout << "The entire condition is false.";
	}

	if (x == 0)
	{
		std::cout << "The value of x is equal to 0.";
	}

	int xx = 3;
	switch (xx)
	{
	case 1:
		std::cout << "The value of x is 1.";
		break;
	case 2:
		std::cout << "The value of x is 2.";
		break;
	case 3:
		std::cout << "The value of x is 3."; // this statement will be
		// executed
		break;
	default:
		std::cout << "The value is none of the above.";
		break;
	}

	for (int i = 0; i < 10; i++)
	{
		std::cout << "The counter is: " << i << '\n';
	}

	x = 0;
	while (x < 10)
	{
		std::cout << "The value of x is: " << x << '\n';
		x++;
	}

	x = 0;
	do
	{
		std::cout << "The value of x is: " << x << '\n';
		x++;
	} while (x < 10);
}