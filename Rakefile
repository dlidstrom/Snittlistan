require 'rake'
require 'rspec/core/rake_task'

# task :default => [:spec]

begin
  require 'tasks/standalone_migrations'
  MigratorTasks.new do |t|
    # t.migrations = "db/migrations"
    # t.config = "db/config.yml"
    # t.schema = "db/schema.rb"
    # t.env = "DB"
    # t.default_env = "development"
    # t.verbose = true
    # t.log_level = Logger::ERROR
  end
rescue LoadError => e
  puts "gem install standalone_migrations to get db:migrate:* tasks! (Error: #{e})"
end
