import { Component, OnInit } from '@angular/core';
import { TaskService } from '../../services/task.service';
import { Task } from '../../models/task.model';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-task-list',
  templateUrl: './task-list.component.html',
  styleUrls: ['./task-list.component.scss'],
})
export class TaskListComponent implements OnInit {
  tasks: Task[] = [];

  constructor(
    private taskService: TaskService,
    private authService: AuthService,
    private router: Router,
    private snackBar: MatSnackBar
  ) {}

  ngOnInit() {
    this.loadTasks();
  }

  loadTasks() {
    this.taskService.getTasks().subscribe({
      next: (tasks) => (this.tasks = tasks),
      error: (error) => console.error(error),
    });
  }

  toggleCompletion(task: Task) {
    this.taskService.updateTaskStatus(task.taskId, !task.isCompleted).subscribe({
      next: () => {
        task.isCompleted = !task.isCompleted;
        this.snackBar.open('Task status updated', 'Close', { duration: 2000 });
      },
      error: (error) => console.error(error),
    });
  }

  viewDetails(task: Task) {
    this.router.navigate(['/tasks/details', task.taskId]);
  }

  addTask() {
    this.router.navigate(['/tasks/new']);
  }

  editTask(task: Task) {
    this.router.navigate(['/tasks/edit', task.taskId]);
  }

  deleteTask(task: Task) {
    if (confirm('Are you sure you want to delete this task?')) {
      this.taskService.deleteTask(task.taskId).subscribe({
        next: () => {
          this.tasks = this.tasks.filter((t) => t.taskId !== task.taskId);
          this.snackBar.open('Task deleted', 'Close', { duration: 2000 });
        },
        error: (error) => console.error(error),
      });
    }
  }

  logout() {
    this.authService.logout();
  }
}
