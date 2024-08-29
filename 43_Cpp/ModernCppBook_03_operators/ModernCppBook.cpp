#include <iostream>

int main()
{
    char mychar = 'c'; // define a char variable mychar
    mychar = 'd'; // assign a new value to mychar
    int x = 123; // define an integer variable x
    x = 456; // assign a new value to x
    int y = 789; // define a new integer variable y
    y = x; // assign a value of x to it

    int result = x + y; // addition
    result = x - y; // subtraction
    result = x * y; // multiplication
    result = x / y; // division
    std::cout << "The result is: " << result << '\n';
    double res2 = x / y;
    std::cout << "The division result is: " << res2 << '\n';

    x += 10; // the same as x = x + 10
    x -= 10; // the same as x = x - 10
    x *= 2; // the same as x = x * 2
    x /= 3; // the same as x = x / 3

    x++; // add 1 to the value of x
    ++x; // add 1 to the value of x
    --x; // decrement the value of x by 1
    x--; // decrement the value of x by 1
}