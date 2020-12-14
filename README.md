# WP DB FIX

Quick and dirty tool to fix WP migration issues from a MySql DB dump.

WordPress [serialized data](https://wpengine.com/support/wordpress-serialized-data/) poses issues when migrating a DB from one URL into another. These come from the fact that PHP serializes arrays using its [serialize function](https://www.w3schools.com/php/phptryit.asp?filename=tryphp_func_var_serialize), which outputs each string prefixed with its length. When the string changes, so must the length.

This tiny tool replaces all the strings in PHP serialized arrays correctly. Please notice that after replacing using this program, you should manually replace any other occurrence of the URL which happens to be non-serialized.

Syntax:

```ps1
./wpdbfix.exe <ReplacementPath> <InputPath> <OutputPath>
```

Sample:

```ps1
./wpdbfix.exe c:\users\dfusi\desktop\rep.txt c:\users\dfusi\desktop\db.sql c:\users\dfusi\desktop\dbr.sql
```

Sample replacements file:

```txt
:leocam19.dreamhosters.com
=iconticonlastoria.it
```

Once replaced, manually replace the remaining occurrences outside serialized arrays.
