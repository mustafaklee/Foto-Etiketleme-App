<template>
  <ul>
    <TreeNode
      v-for="node in treeData"
      :key="node.id"
      :node="node"
      :folderTagData="folderTagData"
      @toggle="handleToggle"
      @selectFolder="$emit('selectFolder', $event)"
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
    folderTagData: Object
  },
  methods: {
    handleToggle(node) {
      node.expanded = !node.expanded;
    },
  },
};
</script>
