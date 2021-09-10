# VU Blockchain Hashing Algorithm

A hashing algorithm designed to emulate all basic requirements of a good hashing algorithm as closely as possible (deterministic, defined range, avalanche-effect based).

# VU Blockchain Hashing Algorithm

## What it does

A hashing algorithm designed to emulate all basic requirements of a good hashing algorithm as closely as possible (deterministic, defined range, avalanche-effect based).

## How it works

### String to mutated unsigned integer conversion

The hashing function works by taking all characters of a string one by one and converting them to their unsigned integer UTF-8 representation. Bitwise operations are then performed in the order of rightwise and leftwise rotations and XOR. The current character that is being operated on is also affected by the character before it (in case of it being the first character in a given string, it is simply operated on by itself), leading to a more consistent implementation of the *Avalanche effect*. All bitwise rotations are done with multiples of a prime number in order to increase the security of the hash and ensure less collissions. All the characters of the string are then obtained as mutated unsigned integers and added together as a sum.

### Mutated unsigned integer to character representation logic

After the sum is obtained, a char list is defined. Currently, it is composed of uppercase letters A-Z, as well as numbers 1-9. This implementation, of course, can be expanded further to include numbers, and, if needed, can be made to support match case, with the potential of making hashes with uppercase and lowercase letters able to be considered different. 

### Creating the actual hash

A for loop is utilized to create the 256-bit hash. A „salt“ is formed for each byte by checking whether the current index in process is smaller than the length of our obtained mutated unsigned integer converted to a character array. 

1. If it is not, our salt is obtained by taking the current character at the index in process and to it, a value of the index times a large prime number is added in order to add for variability. 
2. If it is, our salt is obtained by taking the current character at the remainder, obtained by dividing the index in process by our converted character array's size, plus value of the index times a large prime number which is not the same as the one used in path 1.

If our salt is larger than the length of our converted character array's size, the remainder obtained by dividing the salt from our converted character array is taken. Finally, the obtained number, the „salt“, is used as an index to pick a value from our previously defined char list and the obtained value is appended to the hash. Once this process is repeated a total of 64 times, our desired 256-bit hashed string is returned.