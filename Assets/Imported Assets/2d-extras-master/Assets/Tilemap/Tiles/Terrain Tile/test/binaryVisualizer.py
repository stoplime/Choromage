
import math

def int2bits(n):
    bits = []
    nrange = int(math.log2(n)) + 1
    # print("nrange", nrange)
    for i in reversed(range(nrange)):
        print(n, 2**i)
        if n >= 2**i:
            n -= 2**i
            bits.append(1)
        else:
            bits.append(0)
    # print(bits)
    return bits

def gridVisualizer(bits):
    grid = [["O" for j in range(3)] for i in range(3)]
    grid[1][1] = "H"
    for i, bit in enumerate(reversed(bits)):
        if i == 0 and bit == 1:
            grid[0][1] = "X"
        if i == 1 and bit == 1:
            grid[0][2] = "X"
        if i == 2 and bit == 1:
            grid[1][2] = "X"
        if i == 3 and bit == 1:
            grid[2][2] = "X"
        if i == 4 and bit == 1:
            grid[2][1] = "X"
        if i == 5 and bit == 1:
            grid[2][0] = "X"
        if i == 6 and bit == 1:
            grid[1][0] = "X"
        if i == 7 and bit == 1:
            grid[0][0] = "X"
    printGrid(grid)

def printGrid(grid):
    string = "--------------\n"
    for row in grid:
        for cell in row:
            string += cell + " "
        string += "\n"
    print(string)

if __name__ == "__main__":
    gridVisualizer(int2bits(223))
