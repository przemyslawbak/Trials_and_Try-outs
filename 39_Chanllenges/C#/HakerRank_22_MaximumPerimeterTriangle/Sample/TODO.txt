Given an array of stick lengths, use of them to construct a non-degenerate triangle with the maximum possible perimeter. Return an array of the lengths of its sides as

integers in non-decreasing order.

If there are several valid triangles having the maximum perimeter:

    Choose the one with the longest maximum side.
    If more than one has that maximum, choose from them the one with the longest minimum side.
    If more than one has that maximum as well, print any one them.

If no non-degenerate triangle exists, return

.

Example
The triplet will not form a triangle. Neither will or , so the problem is reduced to and . The longer perimeter is

.

Function Description

Complete the maximumPerimeterTriangle function in the editor below.

maximumPerimeterTriangle has the following parameter(s):

    int sticks[n]: the lengths of sticks available

Returns

    int[3] or int[1]: the side lengths of the chosen triangle in non-decreasing order or -1

Input Format

The first line contains single integer
, the size of array .
The second line contains space-separated integers

, each a stick length.

Constraints

Explanation

Sample Case 0:
There are

possible unique triangles:

The second triangle has the largest perimeter, so we print its side lengths on a new line in non-decreasing order.

Sample Case 1:
The triangle
is degenerate and thus can't be constructed, so we print -1 on a new line.