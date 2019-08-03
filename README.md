# MSGitRepos
a lightweight windows desktop app to retrieve the most starred Github Repositories in the last 30 days with pagination.

## Why
I have created this app as a reponse to United Remote Font-End Challenge: https://github.com/hiddenfounders/frontend-coding-challenge

**Due to the nature of the challenge being merely Code Quality I have skipped optimization and loading speed.**

## Features
* List the most starred Github repos that were created in the last 30 days. 
* Results as a list. One repository per row. 
* For each repo/row the following details :
  * Repository name
  * Repository description 
  * Number of stars for the repo. 
  * Number of issues for the repo.
  * Username and avatar of the owner. 
* Ability to keep scrolling and pagination functionality.

## How Does It Work?

MSGitRepos asynchronously grabs repos from the Github API: `https://api.github.com/search/repositories?q=created:>2017-10-22&sort=stars&order=desc&page=1`and parses results into a listing similar to GIthub trending `https://github.com/trending`
