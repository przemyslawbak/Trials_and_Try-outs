#include <iostream>
enum MyEnum
{
	myfirstvalue,
	mysecondvalue,
	mythirdvalue
};

template <typename T>
class MyClass {
private:
	T x;
public:
	MyClass(T xx)
		:x{ xx }
	{
	}
	T getvalue()
	{
		return x;
	}
};

class MyBaseClass
{
public:
	virtual void dowork()
	{
		std::cout << "Hello from a base class." << '\n';
	}
};

class MyDerivedClass : public MyBaseClass
{
public:
	char c;
	int* x;
	double d;

	void dosomething()
	{
		std::cout << "Hello World from a class.";
	}

	~MyDerivedClass() //destructor
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

template <typename T>
void myfunction(T param)
{
	std::cout << "The value of a parameter is: " << param;
}

int main()
{
	MyBaseClass* f = new MyDerivedClass;
	f->dowork();
	delete f;

	myfunction<int>(123);
	myfunction<double>(123.456);
	myfunction<char>('A');

	MyClass<int> o{ 123 };
	std::cout << "The value of x is: " << o.getvalue() << '\n';
	MyClass<double> o2{ 456.789 };
	std::cout << "The value of x is: " << o2.getvalue() << '\n';

	MyEnum myenum = myfirstvalue;
}
