---
title: DRPTranslator
author: Angel Munoz
published: "2016-03-18"
layout: post
summary: A brief explanation of my first and new fresh typescript Library!
tags: 
 - Typescript
 - Javascript
 - Node
category: 
 - Library
---

# Hello everyone!
This time I come to you to give you a brief explanation of [DRPTranslator](https://github.com/AngelMunoz/DRPTranslator), available in
the [npm registry](https://npmjs.com/package/drptranslator).

This is the first library I've written in my developer's life it was quite interesting to develop and a great experience
so, *What is this library for?* this is aimed at a entry level of genetics say convert DNA sequences into RNA sequences or Aminoacid sequences.
this is based in the [Central Dogma of Molecular Biology](https://en.wikipedia.org/wiki/Central_dogma_of_molecular_biology).
which basically explains how is possible for the nature to replicate itself an keep growing and evolving. when DNA replicates, there are certain phases Transcription and Translation, which is part of what this library is for.

There are way more complex libraries for this in other languages, perhaps in Python or Java or even in F#, for the main purpose of this library is to help you build education software upon it, perhaps you have a friend or a colleague that are having or teaching this class and you find a handy way to use this library to build an app for them then Feel free to do so the library is MIT licensed so feel free to go.

## Installation
Now with the good stuff to install this you can pull it directly from the npm registry save it as a dependency for your nodejs project!

`npm install --save drptranslator`

this library doesn't have any hard dependency on node compoents so if you want to try it on a browser app, you can always use browserify or webpack.

### DNA -> DNA
If you have a DNA sequence that you want to get the complementary chain this is for you!
```javascript
// DNA Sequence
// ATCGGCTAGCTAGCGCTAGCTAGAACGAGT
var drptranslator = require('drptranslator/drptranslator');
var DNATranslator = drptranslator.DNATranslator;
var dnaTrans = new DNATranslator();
var complementaryDNA = dnaTrans.transDNAtoDNA("ATCGGCTAGCTAGCGCTAGCTAGAACGAGT");
console.log(complementaryDNA);
// original     : ATCGGCTAGCTAGCGCTAGCTAGAACGAGT
// complementary: TAGCCGATCGATCGCGATCGATCTTGCTCA
```

### DNA -> RNA
Now, let's use the same DNA sequence to get the RNA complementary sequence which is called Translation in the Central Dogma of Molecular Biology
```javascript
var drptranslator = require('drptranslator/drptranslator');
var DNATranslator = drptranslator.DNATranslator;
var dnaTrans = new DNATranslator();
var complementaryDNA = dnaTrans.transDNAtoRNA("ATCGGCTAGCTAGCGCTAGCTAGAACGAGT");
console.log(complementaryDNA);
// original     : ATCGGCTAGCTAGCGCTAGCTAGAACGAGT
// complementary: UAGCCGAUCGAUCGCGAUCGAUCUUGCUCA
```

### DNA -> AA
So we've done some Translation what about transcription?
this is where it gets good. there's an enzyme in the body dedicated to that (I'm pretty sure it's not the only one) transcription is the process of converting DNA into a protein, proteins are made of aminoacids so how do we go there?

once we have our RNA sequence `UAGCCGAUCGAUCGCGAUCGAUCUUGCUCA` we have to look for a sequence that has a Start codon and a stop codon, what is a codon you may ask? it's a group of three bases
`UAG CCG AUC GAU CGC GAU CGA UCU UGC UCA` we have 10 codons here
now one thing the library does is to check for start and stop codons to try to accurately transcript the sequences
so let's test our transcription
```javascript
var drptranslator = require('drptranslator/drptranslator');
var DNATranslator = drptranslator.DNATranslator;
var dnaTrans = new DNATranslator();
var complementaryDNA = dnaTrans.transDNAtoRNA("ATCGGCTAGCTAGCGCTAGCTAGAACGAGT");
console.log(complementaryDNA);
// original     : ATCGGCTAGCTAGCGCTAGCTAGAACGAGT
// complementary: STOP
```
Wait What!? it just outputs STOP? well when I put the sequence I didn't realize that I was puting an STOP sequence First `UAG` so if you have a chain with multiple stops it will chop the sequence on the first stop, I will try to add the option to translate a sequence with multiple starts and stops later on, but right now that's the default behavior.

So let's try again without our sequence but without the stop codon
```javascript
var drptranslator = require('drptranslator/drptranslator');
var DNATranslator = drptranslator.DNATranslator;
var dnaTrans = new DNATranslator();
var complementaryDNA = dnaTrans.transDNAtoRNA("GGCTAGCTAGCGCTAGCTAGAACGAGT");
console.log(complementaryDNA);
// original     : GGCTAGCTAGCGCTAGCTAGAACGAGT
// complementary: Pro-Ile-Asp-Arg-Asp-Arg-Ser-Cys-Ser
```
that looks better right?
the way the codons are identified is with a [**Genetic Code Table** ](https://en.wikipedia.org/wiki/Genetic_code)

### RNA -> ?
Well RNATranslator class is used to let DNATranslator class to work, so If you want to translate or transcript RNA you can do it as well, but instead requiring a DNA sequence you need a RNA sequence
```javascript
var drptranslator = require('drptranslator/drptranslator');
var RNATranslator = drptranslator.RNATranslator;
var rnaTrans = new RNATranslator();

var rnatodna = rnaTrans.transRNAtoDNA("CCGAUCGAUCGCGAUCGAUCUUGCUCA");
var aaSeq = rnaTrans.transRNAtoAA("CCGAUCGAUCGCGAUCGAUCUUGCUCA");
console.log(aaSeq, rnatodna);
// Pro-Ile-Asp-Arg-Asp-Arg-Ser-Cys-Ser
// GGCTAGCTAGCGCTAGCTAGAACGAGT
```

These are the most basic uses of the library, however there are ways to detect starts and stops, and so some other things with RNA, but I will document those in the repository.

Please enjoy this library and if you want to contribute or have a suggestion, please raise an issue in the [repository](https://github.com/AngelMunoz/DRPTranslator)
