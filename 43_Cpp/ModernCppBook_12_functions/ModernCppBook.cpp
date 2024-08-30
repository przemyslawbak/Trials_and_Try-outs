#include <iostream>

void myfunction() {
	std::cout << "Hello World from a function.";
}

int mysquarednumber(int x) {
	return x * x;
}

void myfunction(int& byreference)
{
	byreference++; // we can modify the value of the argument
	std::cout << "An argument passed by reference: " << byreference;
}

int mysum(int x, int y);

int main()
{
	myfunction(); // a call to a function

	int myresult = mysquarednumber(2); // a call to the function
	std::cout << "Number 2 squared is: " << myresult;

	int rr = mysum(5, 10);
	std::cout << "The sum of 5 and 10 is: " << rr;

	int x = 123;
	myfunction(x);
}

int mysum(int x, int y) {
	return x + y;
}
