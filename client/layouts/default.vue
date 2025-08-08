<script setup lang="ts">
import { provide, ref, onMounted, watch } from "vue";
import { useDisplay } from "vuetify";
import { fetchGetApi, fetchPostApi } from "~/server/api";
import type { TagType } from "~/server/api-schema";

const { mdAndDown } = useDisplay();

const drawerOpen = ref(false);

const tagName = ref("");
const tags = ref<TagType[]>([]);
const selectedTag = ref<number | null>(null);

provide("tags", tags);
provide("selectedTag", selectedTag);

onMounted(() => {
  getTags();
  drawerOpen.value = !mdAndDown.value; 
});

watch(mdAndDown, (val) => {
  drawerOpen.value = !val;
});

function selectTag(id: number) {
  selectedTag.value = id;
  navigateTo("/");
  if (mdAndDown.value) drawerOpen.value = false; 
}

const getCalendar = async () => {
  navigateTo("/calendar");
  if (mdAndDown.value) drawerOpen.value = false;
};
const getCategories = async () => {
  navigateTo("/tags");
  if (mdAndDown.value) drawerOpen.value = false;
};
const getSubtasks = async () => {
  navigateTo("/subtasks");
  if (mdAndDown.value) drawerOpen.value = false;
};
const postTag = async () => {
  const newTag: TagType = {
    tagId: 0,
    tagName: tagName.value,
  };

  try {
    const response = await fetchPostApi<TagType>("/Tags", newTag);
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
provide("refreshTags", getTags);
</script>

<template>
  <v-app>
    <v-app-bar app>
      <v-app-bar-nav-icon v-if="mdAndDown" @click="drawerOpen = !drawerOpen" />
      <v-toolbar-title>My Task App</v-toolbar-title>
    </v-app-bar>

    <v-navigation-drawer
      app
      v-model="drawerOpen"
      :permanent="!mdAndDown"
      :temporary="mdAndDown"
      width="280"
    >
      <v-list>
        <v-list-item @click="selectTag(0)">
          <v-list-item-title>ALL TASKS</v-list-item-title>
        </v-list-item>

        <v-list-item @click="getSubtasks()">
          <v-list-item-title>ALL SUBTASKS</v-list-item-title>
        </v-list-item>

        <v-list-item @click="getCategories()">
          <v-list-item-title>TAGS</v-list-item-title>
        </v-list-item>

        <v-list-item @click="getCalendar()">
          <v-list-item-title>CALENDAR</v-list-item-title>
        </v-list-item>

        <v-divider></v-divider>

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

    <!-- Main content -->
    <v-main>
      <slot />
    </v-main>
  </v-app>
</template>
