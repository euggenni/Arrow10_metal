#!/bin/bash

find . -type f -path '*.cs' -exec bash -c '
  for file do
  	encoding="$(file -b --mime-encoding "$file")"
    if [[ "$encoding" == "iso-8859-1" || "$encoding" == "unknown-8bit" ]]; then
    	echo -ne \\xEF\\xBB\\xBF > "$file.conv"
    	iconv -f windows-1251 -t utf-8 "$file" >> "$file.conv"
    	status=$?
    	if [[ $status == 0 ]]; then
    		mv -f "$file.conv" "$file"
    		status=$?
    	fi

    	if [[ $status == 0 ]]; then
    		echo "$file"
    	else
    		echo "Error $file"
    	fi
    fi
  done' bash {} +