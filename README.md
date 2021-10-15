# Documentation
###### Source code: https://github.com/caprapaul/flcd/tree/lab_02
### Symbol Table

Implemented as a hash table using an array of lists. The lists are used to handle collisions.

#### Hash(symbol)
Computes a hash code by adding the ASCII code of the symbol's characters.
**in**: *symbol*: a symbol
**out**: a hash code for the specified key argument

#### Add(symbol)
Adds a symbol to the table.

**in**: *symbol*: a symbol

**out**: an object containing the hash and the index of the added symbol or null if the symbol already exists

#### FindPosition(symbol)
Finds the position of a symbol in the table.

**in**: *symbol*: a symbol

**out**: an object containing the hash and the index of the added symbol or null if the symbol does not exist
