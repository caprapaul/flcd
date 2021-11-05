# Documentation
###### Source code: https://github.com/caprapaul/flcd/tree/lab_04

## Finite Automata

The FA is read from a json file. 

To check if it is deterministic we go through each list of transitions in the `Transititions` dictionary and check if it contains elements with the same label.

To verify if a sequence is accepted by the FA we start from the initial state and go the next state using the `Transitions` dictionary.

### EBNF

```
fa ::= "{" states "," alphabet "," intial_state "," final_states "," transitions "}"

states ::= '"States"' ":" list
alphabet ::= '"Alphabet"' ":" list
initial_state ::= '"InitialState"' ":" string
final_states ::= '"FinalStates"' ":" list
transitions ::= '"Transitions"' ":" transitions_dictionary
transitions_dictionary ::= "{" [transitions_dictionary_items] "}"
transitions_dictionary_items ::= transition | transition "," transition
transition ::= string ":" "[" [transition_items] "]"
transition_items ::= transition_item | transition_item "," transition_item
transition_item ::= "{" '"ToState"' ":" string "," '"Label"' ":" string "}"

list ::= "[" [list_items]  "]"
list_items ::= string | string "," string

string ::= '"' {char} '"'
char ::= letter | digit
letter ::= "a" | ... | "z" | "A" | ... | "Z"
digit ::= "0" | ... | "9"

```

## Classes

![image-20211105155425059](E:\Users\user\AppData\Roaming\Typora\typora-user-images\image-20211105155425059.png)
