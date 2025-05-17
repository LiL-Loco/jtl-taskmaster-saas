'use client'

import { useQuery } from '@tanstack/react-query'
import {
  Table,
  TableBody,
  TableCell,
  TableHead,
  TableHeader,
  TableRow,
} from '@/components/ui/table'
import { Button } from '@/components/ui/button'
import { Card, CardContent, CardHeader, CardTitle } from '@/components/ui/card'
import { jobsApi } from '@/services/api'

export function JobList() {
  const { data: jobs, isLoading } = useQuery({
    queryKey: ['jobs'],
    queryFn: () => jobsApi.getAll()
  })

  if (isLoading) {
    return <div>Loading...</div>
  }

  return (
    <Card>
      <CardHeader className="flex flex-row items-center justify-between">
        <CardTitle>Jobs</CardTitle>
        <Button>New Job</Button>
      </CardHeader>
      <CardContent>
        <Table>
          <TableHeader>
            <TableRow>
              <TableHead>Name</TableHead>
              <TableHead>Status</TableHead>
              <TableHead>Last Run</TableHead>
              <TableHead>Actions</TableHead>
            </TableRow>
          </TableHeader>
          <TableBody>
            {jobs?.map((job) => (
              <TableRow key={job.id}>
                <TableCell>{job.name}</TableCell>
                <TableCell>{job.status}</TableCell>
                <TableCell>{job.lastRun}</TableCell>
                <TableCell>
                  <Button variant="ghost" size="sm">Edit</Button>
                  <Button variant="ghost" size="sm">Start</Button>
                  <Button variant="ghost" size="sm" className="text-red-600">Delete</Button>
                </TableCell>
              </TableRow>
            ))}
          </TableBody>
        </Table>
      </CardContent>
    </Card>
  )
}