## Synonyms

**Frontend**: React\
**Backend**: C#

Application for storing and searching the synonyms of words using C# IMemoryCache. 

There are three pages.\
+ Home page - page where you can search for synonyms of a specific word
+ All words - page where you can see all the words in cache memory
+ Add new - page where you can add new word and its synonyms.

When adding new word, system checks if it already exists, and applies the transitive rule if necessary.\
**Transitive rule**: "B" is a synonym to "A" and "C" a synonym to "B", then "C" should automatically, by transitive rule, also be the synonym for "A".

Added form validation. Implemented backend validation as well (checking for empty or duplicated words, or checking if user added a word that is a synonym to itself).\
Added unit tests for both valid and invalid inputs.

Added swagger to easily view and use API methods.

## Installation and setup

### Frontend:
Install packages: npm i\
Run the app: npm start

Runs the app in the development mode.\
Open [http://localhost:3000](http://localhost:3000) to view it in your browser.

The page will reload when you make changes.\
You may also see any lint errors in the console.

### BackEnd
No configuration or installation needed. Just run the application.


### End
