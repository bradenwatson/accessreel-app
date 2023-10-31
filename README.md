Update a local branch with changes from a remote repository:
1. Open GitBash in local repo directory.
2. Choose remote repo's name.
3. Add remote repo to local repo:
git remote add <remote-name> <remote-repository-URL>
4. Get the branch name of the remote repo that changes are being pulled from.
5. Pull changes from the branch of the remote repo and merge them into the local branch:
git pull <remote-name> <remote-branch-name>

Integrate changes from remote repo's branch into local branch and push those changes to another repository's branch:
1. Fetch and merge changes from a branch in the named repository into your local branch:
git pull <named-repository-name> <named-repository-branch-name>:<local-branch-name>
2. Add the other repository as a remote named "new-origin":
git remote add new-origin <other-repository-URL>
3. Push your local branch to the branch in the "new-origin" remote repository:
git push new-origin <local-branch-name>:<local-branch-name>
