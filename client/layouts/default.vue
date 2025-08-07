<!-- layouts/default.vue -->
<template>
  <v-app>
    <v-navigation-drawer app>
      <v-list>
        <v-list-item
          v-for="tag in tags"
          :key="tag.tagId"
          :value="tag.tagId"
          @click="selectTag(tag.tagId)"
          :active="selectedTag === tag.tagId"
        >
          <v-list-item-title>{{ tag.tagName }}</v-list-item-title>
        </v-list-item>

        <v-list-item>
          <div style="display: flex; gap: 8px">
            <v-text-field v-model="tagName" label="New Tag" outlined dense />
            <v-btn icon @click="postTag">
              <v-icon>mdi-plus</v-icon>
            </v-btn>
          </div>
        </v-list-item>
      </v-list>
    </v-navigation-drawer>

    <v-main>
      <slot />
    </v-main>
  </v-app>
</template>

<script setup lang="ts">
import { provide, ref, onMounted } from "vue";
import { fetchGetApi, fetchPostApi } from "~/server/api";
import type { TagType } from "~/server/api-schema";

const tagName = ref("");
const tags = ref<TagType[]>([]);
const selectedTag = ref<number | null>(null);

provide("tags", tags);
provide("selectedTag", selectedTag);

onMounted(() => {
  getTags();
});

function selectTag(id: number) {
  selectedTag.value = id;
}

const postTag = async () => {
  const newTag: TagType = {
    tagId:0,
    tagName: tagName.value,
  };

  try {
    const response =await fetchPostApi<TagType>("/Tags", newTag);
    tags.value.push(response);
  } catch (err) {
    console.error("Error saving tag:", err);
  }
};

const getTags = async () => {
  try {
    const response = await fetchGetApi<TagType[]>("/Tags");
    tags.value = response;
  } catch (err) {
    console.error("Error fetching tags:", err);
  }
};
</script>
