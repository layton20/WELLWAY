import Box from '@mui/material/Box'
import { DataGrid } from '@mui/x-data-grid'
import type {
  GridColDef,
  GridRowParams,
  GridValidRowModel,
  GridPaginationModel,
} from '@mui/x-data-grid'
import { WWSkeleton } from '../feedback/WWSkeleton'

function LoadingOverlay() {
  return (
    <Box sx={{ p: 2, display: 'flex', flexDirection: 'column', gap: 1 }}>
      <WWSkeleton variant="rectangular" height={48} count={5} />
    </Box>
  )
}

interface WWDataTableProps<TRow extends GridValidRowModel> {
  rows: TRow[]
  columns: GridColDef<TRow>[]
  loading?: boolean
  onRowClick?: (row: TRow) => void
  totalCount?: number
  pageSize?: number
  page?: number
  onPageChange?: (page: number) => void
}

export function WWDataTable<TRow extends GridValidRowModel>({
  rows,
  columns,
  loading = false,
  onRowClick,
  totalCount,
  pageSize = 25,
  page = 0,
  onPageChange,
}: WWDataTableProps<TRow>) {
  const paginationModel: GridPaginationModel = { page, pageSize }

  return (
    <DataGrid<TRow>
      rows={rows}
      columns={columns}
      loading={loading}
      autoHeight
      density="comfortable"
      paginationMode="server"
      rowCount={totalCount ?? rows.length}
      paginationModel={paginationModel}
      onPaginationModelChange={(model) => onPageChange?.(model.page)}
      pageSizeOptions={[pageSize]}
      onRowClick={(params: GridRowParams<TRow>) => onRowClick?.(params.row)}
      slots={{ loadingOverlay: LoadingOverlay }}
      sx={{ border: 'none' }}
    />
  )
}
