<script lang="ts" setup>
import { ref, onMounted, inject } from "vue";
import {
  fetchGetApi,
  fetchPutApi,
  fetchDeleteApi,
  fetchPostApi,
} from "~/server/api";
import type { TagType } from "~/server/api-schema";

const tags = ref<TagType[]>([]);

const editDialog = ref(false);
const editTagData = ref<TagType>({
  tagId: 0,
  tagName: "",
  tagColor: "#1976d2",
});

const addDialog = ref(false);
const newTagData = ref<TagType>({
  tagId: 0,
  tagName: "",
  tagColor: "#1976d2",
});

const deleteDialog = ref(false);
const tagToDelete = ref<TagType | null>(null);

const refreshTags = inject<() => Promise<void>>("refreshTags");

onMounted(() => {
  loadTags();
});

const loadTags = async () => {
  tags.value = await fetchGetApi<TagType[]>("/Tags");
};

const openEdit = (tag: TagType) => {
  editTagData.value = { ...tag };
  editDialog.value = true;
};

const saveEdit = async () => {
  await fetchPutApi(editTagData.value.tagId, "/Tags", editTagData.value);
  editDialog.value = false;
  await loadTags();
  await refreshTags?.();
};

const openAddDialog = () => {
  newTagData.value = { tagId: 0, tagName: "", tagColor: "#1976d2" };
  addDialog.value = true;
};

const saveAdd = async () => {
  if (!newTagData.value.tagName.trim()) return;
  await fetchPostApi<TagType>("/Tags", newTagData.value);
  addDialog.value = false;
  await loadTags();
  await refreshTags?.();
};

const openDeleteDialog = (tag: TagType) => {
  tagToDelete.value = tag;
  deleteDialog.value = true;
};

const confirmDelete = async () => {
  if (!tagToDelete.value) return;
  await fetchDeleteApi(tagToDelete.value.tagId, "/Tags");
  deleteDialog.value = false;
  tagToDelete.value = null;
  await refreshTags?.();
  await loadTags();
};
</script>

<template>
  <div class="pa-4">
    <h1 class="text-h5">Tags</h1>
    <v-divider class="mb-4"></v-divider>

    <v-btn color="primary" class="mb-4" @click="openAddDialog">Add New Tag</v-btn>

    <v-list>
      <!-- Tags List -->
      <v-list-item v-for="tag in tags" :key="`tag-${tag.tagId}`" class="pa-0">
        <v-card>
          <v-card-text>
            <div
              style="
                display: flex;
                justify-content: space-between;
                align-items: center;
              "
            >
              <div style="display: flex; align-items: center">
                <v-avatar
                  size="24"
                  :color="tag.tagColor || 'primary'"
                  class="mr-2"
                ></v-avatar>
                <span>{{ tag.tagName }}</span>
              </div>
              <div>
                <v-btn class="mr-2" icon size="small" @click="openEdit(tag)">
                  <v-icon>mdi-pencil</v-icon>
                </v-btn>
                <v-btn
                  icon
                  size="small"
                  color="red"
                  @click="openDeleteDialog(tag)"
                >
                  <v-icon>mdi-delete</v-icon>
                </v-btn>
              </div>
            </div>
          </v-card-text>
        </v-card>
      </v-list-item>
    </v-list>

    <!-- Dialog to edit tag -->
    <v-dialog v-model="editDialog" max-width="400">
      <v-card>
        <v-card-title>Edit Tag</v-card-title>
        <v-card-text>
          <v-text-field v-model="editTagData.tagName" label="Name" />
          <v-color-picker
            v-model="editTagData.tagColor"
            flat
            hide-canvas
            hide-inputs
            show-swatches
            swatches-max-height="120"
          />
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn text @click="editDialog = false">Cancel</v-btn>
          <v-btn color="primary" text @click="saveEdit">Save</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- Dialog to create tag -->
    <v-dialog v-model="addDialog" max-width="400">
      <v-card>
        <v-card-title>New Tag</v-card-title>
        <v-card-text>
          <v-text-field v-model="newTagData.tagName" label="Name" />
          <v-color-picker
            v-model="newTagData.tagColor"
            flat
            hide-canvas
            hide-inputs
            show-swatches
            swatches-max-height="120"
          />
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn text @click="addDialog = false">Cancel</v-btn>
          <v-btn color="primary" text @click="saveAdd">Add</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>

    <!-- Dialog to confirm delete tag -->
    <v-dialog v-model="deleteDialog" max-width="400">
      <v-card>
        <v-card-title class="text-h6">Confirm Deletion</v-card-title>
        <v-card-text>
          Are you sure you want to delete the tag "<strong>{{
            tagToDelete?.tagName
          }}</strong
          >"?
        </v-card-text>
        <v-card-actions>
          <v-spacer />
          <v-btn text @click="deleteDialog = false">Cancel</v-btn>
          <v-btn color="red" text @click="confirmDelete">Delete</v-btn>
        </v-card-actions>
      </v-card>
    </v-dialog>
  </div>
</template>
