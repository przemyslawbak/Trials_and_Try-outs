#include <iostream>

class MyClass
{
public:
	char c;
	int* x;
	double d;

	void dosomething()
	{
		std::cout << "Hello World from a class.";
	}

	~MyClass() //destructor
	{
		delete x;
		std::cout << "Deleted a pointer in the destructor." << '\n';
	}

protected:
	// everything in here
	// has protected access level
private:
	// everything in here
	// has private access level
};

int main()
{
	int* p = new int;
	*p = 123;
	std::cout << "The pointed-to value is: " << *p;
	delete p;

	MyClass o; //constructed
	o.c = 'a';
	o.dosomething();

	MyClass o2 = o;
} //destructed
