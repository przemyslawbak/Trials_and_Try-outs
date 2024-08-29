#include <iostream>

int main()
{
    std::cout << "Please enter a number and press enter: ";
    int x = 0;
    int y = 0;
    char c = 0;
    double d = 0.0;
    std::cin >> x;
    std::cout << "You entered: " << x;

    std::cin >> x >> y;
    std::cout << "You entered: " << x << " and " << y;

    std::cin >> c >> x >> d;
    std::cout << "You entered: " << c << ", " << x << " and " << d;
}