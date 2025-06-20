export function initializeTree(folders, onFolderClick) {
    const treeDiv = document.getElementById("div_tree");
    const starIcon = '/css/images/star.png';
    const keyIcon = '/css/images/blue_key.png';
    const tree = createTree('div_tree', 'white', {});

    folders.forEach(folder => {
        const folderPath = folder.folderPath.split('/').pop();
        const folderNode = tree.createNode(folderPath, false, starIcon, null, null, 'context1');

        folder.fotograflar.forEach(foto => {
            const fileName = foto.path.split('/').pop();
            folderNode.createChildNode(
                fileName,
                false,
                keyIcon,
                () => onFolderClick(folder.fotograflar),
                'context1'
            );
        });
    });

    tree.drawTree();
}
