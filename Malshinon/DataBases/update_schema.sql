-- Add created_at column to intelreports table if it doesn't exist
ALTER TABLE intelreports ADD COLUMN IF NOT EXISTS created_at DATETIME DEFAULT CURRENT_TIMESTAMP;

-- Create alerts table if it doesn't exist
CREATE TABLE IF NOT EXISTS alerts (
    id INT PRIMARY KEY AUTO_INCREMENT,
    target_id INT NOT NULL,
    start_time DATETIME NOT NULL,
    end_time DATETIME NOT NULL,
    reason VARCHAR(255) NOT NULL,
    created_at DATETIME DEFAULT CURRENT_TIMESTAMP,
    FOREIGN KEY (target_id) REFERENCES people(id)
);

-- Add indexes for better performance
CREATE INDEX IF NOT EXISTS idx_intelreports_created_at ON intelreports(created_at);
CREATE INDEX IF NOT EXISTS idx_alerts_created_at ON alerts(created_at);
CREATE INDEX IF NOT EXISTS idx_alerts_target_id ON alerts(target_id);

-- Add potential_agent type to people table if not exists
ALTER TABLE people MODIFY COLUMN type ENUM('reporter', 'target', 'both', 'potential_agent') NOT NULL; 