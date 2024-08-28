#include <iostream>

int main()
{
    std::cout << "Some string." << " Another string.";
    bool b = true;
    //bool c{ true };
    char c = 'a';
    //std::cout << "The value of variable c is: " << c;
    c = 'B';
    //std::cout << " The new value of variable c is: " << c;
    c = 97; //same as 'a'
    int x = 123;
    int y = x;
    std::cout << "The value of x is: " << x << ", the value of y is: " << y;
    // x is 123
    // y is 123
    x = 456;
    std::cout << "The value of x is: " << x << ", the value of y is: " << y;
    // x is now 456
    // y is still 123
    double x = 213.456;
    double y = 1.;
    double z = 0.15;
    double w = .15;
    double d = 3.14e10;
}