#!/bin/bash

for clubId in  `jq '.data | .[] | .clubId' ../../../Clubs.json | sort -un`
do
    echo $clubId
    if [ ! -f clubLogos/$clubId.png ]
    then
        curl -sXGET https://bits.swebowl.se/images/ClubLogo/$clubId.png > clubLogos/$clubId.png
    else
        echo "$clubId.png exists"
    fi

    #read -p 'Press any key to continue'
done
