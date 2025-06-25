<template>
  <li>
    <div class="tree-node-label" @click="toggleNodeOrSelect" :title="node.name">
      <template v-if="node.photoUrl">
        <a :href="node.photoUrl" target="_blank" @click.stop>{{ node.name }}</a>
      </template>
      <template v-else>
        <span class="folder-icon">📁</span>
        {{ node.name }}
      </template>
    </div>
    <ul v-show="expanded" v-if="node.children && node.children.length">
      <TreeNode
        v-for="child in node.children"
        :key="child.id"
        :node="child"
        @toggle="$emit('toggle', $event)"
      />
    </ul>
  </li>
</template>

<script>
export default {
  name: "TreeNode",
  props: {
    node: Object,
  },
  components: {
    TreeNode: () => import("./TreeNode.vue"),
  },
  data() {
    return {
      expanded: !!this.node.expanded,
    };
  },
  methods: {
    toggleNodeOrSelect() {
      if (!this.node.photoUrl) {
        // Klasör tıklandı
        this.$emit('selectFolder', this.node.id);
      }
      if (this.node.children && this.node.children.length) {
        this.expanded = !this.expanded;
        this.$emit("toggle", this.node);
      }
    },
  },
  watch: {
    "node.expanded"(val) {
      this.expanded = val;
    },
  },
};
</script>

<style scoped>
ul {
  padding-left: 0; /* Sola yaslamak için padding kaldırıldı */
  list-style: none;
}
li {
  margin: 0.2rem 0;
  cursor: pointer;
  max-width: 260px; /* Sol panel genişliğine göre ayarla */
}
.tree-node-label {
  display: flex;
  max-width: 240px; /* Sol panel genişliğine göre ayarla */
  white-space: normal;
  word-break: break-all;
  overflow-wrap: anywhere;
  padding: 2px 4px;
  border-radius: 3px;
  transition: background 0.2s;
  align-items: center;
}
.tree-node-label:hover {
  background: #f0f0f0;
}
.folder-icon {
  margin-right: 6px;
}
a {
  color: #1976d2;
  text-decoration: none;
  word-break: break-all;
}
a:hover {
  text-decoration: underline;
}
</style>
