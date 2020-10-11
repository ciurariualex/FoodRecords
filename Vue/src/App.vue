<template>
  <div class="container-fluid mt-4">
    <h1 class="h1">Foods</h1>
    <b-alert :show="loading" variant="info">Loading...</b-alert>
    <b-row>
      <b-col>
        <table class="table table-striped">
          <thead>
            <tr>
              <th>Name</th>
              <th>Ingredients</th>
              <th>Minutes</th>
              <th>Price</th>
            </tr>
          </thead>
          <tbody>
            <tr v-for="record in records" :key="record.id">
              <td>{{ record.name }}</td>
              <td>{{ record.ingredients }}</td>
              <td>{{ record.minutes }}</td>
              <td>{{ record.price }}</td>
              <td class="text-right">
                <a href="#" @click.prevent="updateFood(record)">Edit</a> -
                <a href="#" @click.prevent="deleteFood(record.id)">Delete</a>
              </td>
            </tr>
          </tbody>
        </table>
      </b-col>
      <b-col lg="3">
        <b-card :title="(model.id ? 'Edit Food: ' + model.name : 'Add Food')">
          <form @submit.prevent="createFood">
            <b-form-group label="Name">
              <b-form-input type="text" v-model="model.name"></b-form-input>
            </b-form-group>
            <b-form-group label="Ingredients">
              <b-form-input type="text" v-model="model.ingredients"></b-form-input>
            </b-form-group>
            <b-form-group label="Minutes">
              <b-form-input type="number" v-model="model.minutes"></b-form-input>
            </b-form-group>
            <b-form-group label="Price">
              <b-form-input type="number" v-model="model.price"></b-form-input>
            </b-form-group>
            <div>
              <b-btn type="submit" variant="success">Save</b-btn>
            </div>
          </form>
        </b-card>
      </b-col>
    </b-row>
  </div>
</template>

<script>
  import api from '@/FoodsApiService';

  export default {
    data() {
      return {
        loading: false,
        records: [],
        model: {}
      };
    },
    async created() {
      this.getAll()
    },
    methods: {
      async getAll() {
        this.loading = true

        try {
          this.records = await api.getAll()
        } finally {
          this.loading = false
        }
      },
      async updateFood(food) {
        // We use Object.assign() to create a new (separate) instance
        this.model = Object.assign({}, food)
      },
      async createFood() {
        const isUpdate = !!this.model.id;

        if (isUpdate) {
          await api.update(this.model.id, this.model)
        } else {
          await api.create(this.model)
        }

        // Clear the data inside of the form
        this.model = {}

        // Fetch all records again to have latest data
        await this.getAll()
      },
      async deleteFood(id) {
        if (confirm('Are you sure you want to delete this food?')) {
          if (this.model.id === id) {
            this.model = {}
          }

          await api.delete(id)
          await this.getAll()
        }
      }
    }
  }
</script>