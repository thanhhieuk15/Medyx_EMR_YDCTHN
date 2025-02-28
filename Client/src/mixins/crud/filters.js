export default {
  props: ["filterParams", "categories", "fields"],
  computed: {
    filterFields() {
      return this.fields.filter((f) => f.filter);
    },
  },
  methods: {
    handleSearch(e) {
      this.$emit("filter-change");
    },
    updateFilterParams(key, value) {
      this.$set(this.filterParams, key, value);
    },
    // handleSearch() {
    //   this.handleInputChange();  // Gọi phương thức tìm kiếm ở đây
    // },
  },
};
