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

## Upsides and downsides

### The upsides

1. The algorithm always generates a 256-bit string. 
2. One input always produces identical output.
3. The algorithm is avalanche-effect based (80% average difference), jumbling bits around heavily, meaning similar strings give completely different hashes.
4. It is comparatively fast, relying on bitwise operations and simple index assignments to generate a hex code for the hash.

### The downsides

1. Even if the algorithm is avalanche-effect based, some outputs have been spotted that are up to 90% similar, although such occurences are rare.


## Testing

### Deterministic

The algorithm was checked thoroughly whether it is deterministic. One input always gives the same output, thus the requirement is satisfied.

### Defined range

No outputs have currently been found that produce a hash that is longer than 256-bits. This should, also, be impossible in practice, due to how the algorithm forms strings.

### Avalanche-effect based

The code itself has a test to check the percentage difference of a huge input of randomly generated pairs of strings. The only difference between the two strings is one single character. The average difference during most tests thus far has been around 65-85%, with occasional extremums. This, in my opinion, more than satisfies my initial goal of 75%.

### Fast

I added the ability to benchmark the hashing algorithm. By hashing "konstitucija.txt" repeatedly, I noticed that the average time to hash it is around 5 - 6.5 ms. This, compared to MD5's 8.5 - 10.5 ms and SHA256's 8 - 10 ms is a great performance indication.

### Uniform

The hash should, ideally, use as much of its 64-bit unsigned dataspace, as lots of bit rotations and bitwise operations are performed that should, in theory, spread it uniformly throughout the range.

### Collission-resistant

As was mentioned in the uniformity test, bit rotations and other bitwise operations are performed, which jumble and spread the potential range out. This lowers the risk of two inputs having the same output drastically.