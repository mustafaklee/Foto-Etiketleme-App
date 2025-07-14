<template>
  <ul class="tree-root">
    <TreeNode
      v-for="node in treeData"
      :key="node.id"
      :node="node"
      :folderTagData="folderTagData"
      :selectedFolderId="selectedFolderId"
      @selectFolder="$emit('selectFolder', $event)"
      @toggle="$emit('toggle', $event)"
    />
  </ul>
</template>

<script>
import TreeNode from "./TreeNode.vue";

export default {
  name: "TreeView",
  components: { TreeNode },
  props: {
    treeData: Array,
    folderTagData: Object,
    selectedFolderId: String,
  },
  methods: {
    handleToggle(node) {
      node.expanded = !node.expanded;
    },
  },
};
</script>
